using Core.Configuration;
using Core.Services;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Minio
{
	public class MinioService : IMinioService
	{
		private readonly IMinioClient _minioClient;
		private readonly string _bucketName;

		public MinioService(IOptions<MinioSettings> minioSettings)
		{
			var settings = minioSettings.Value;
			_minioClient = new MinioClient()
				.WithEndpoint(settings.Endpoint)
				.WithCredentials(settings.AccessKey, settings.SecretKey)
				.WithSSL(settings.UseSSL)
				.Build();
			_bucketName = settings.BucketName;
		}

		public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
		{
			try
			{
				// Ensure bucket exists
				bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
				if (!found)
				{
					await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
				}

				// Generate unique file name
				string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

				string contentType = "application/octet-stream";
				if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
					contentType = "image/jpeg";
				else if (fileName.EndsWith(".png"))
					contentType = "image/png";
				else if (fileName.EndsWith(".gif"))
					contentType = "image/gif";

				// Upload file
				await _minioClient.PutObjectAsync(
					new PutObjectArgs()
						.WithBucket(_bucketName)
						.WithObject(uniqueFileName)
						.WithStreamData(fileStream)
						.WithObjectSize(fileStream.Length)
						.WithContentType(contentType)
				);

				// Return only the unique filename
				return uniqueFileName;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error uploading file to MinIO: {ex.Message}", ex);
			}
		}

		public async Task DeleteFileAsync(string fileName)
		{
			try
			{
				await _minioClient.RemoveObjectAsync(
					new RemoveObjectArgs()
						.WithBucket(_bucketName)
						.WithObject(fileName)
				);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error deleting file from MinIO: {ex.Message}", ex);
			}
		}

		public async Task<Stream> GetFileAsync(string fileName)
		{
			try
			{
				var memoryStream = new MemoryStream();
				await _minioClient.GetObjectAsync(
					new GetObjectArgs()
						.WithBucket(_bucketName)
						.WithObject(fileName)
						.WithCallbackStream(stream => stream.CopyTo(memoryStream))
				);
				memoryStream.Position = 0;
				return memoryStream;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error getting file from MinIO: {ex.Message}", ex);
			}
		}
	}
}
