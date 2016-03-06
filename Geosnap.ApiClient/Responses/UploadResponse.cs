using Newtonsoft.Json;
using System;

namespace Geosnap.ApiClient.Responses
{
    public sealed class UploadResponse
    {
        public int Id { get; }
        public string Title { get; }
        public string PicturePath { get; }
        public string ThumbnailPath { get; }
        public PointResponse Point { get; }
        public DateTimeOffset InsertedAt { get; }
        public UserResponse SubmittedBy { get; }
        public PictureCategoryResponse Category { get; }
        public int Score { get; }

        [JsonConstructor]
        public UploadResponse(
            int id, 
            string title, 
            string picture_path, 
            string thumbnail_path, 
            PointResponse point, 
            DateTimeOffset inserted_at, 
            UserResponse submitted_by, 
            PictureCategoryResponse category, 
            int score)
        {
            Id = id;
            Title = title;
            PicturePath = picture_path;
            ThumbnailPath = thumbnail_path;
            Point = point;
            InsertedAt = inserted_at;
            SubmittedBy = submitted_by;
            Category = category;
            Score = score;
        }
    }
}
