using Microsoft.ML;
using Microsoft.ML.Trainers;
using Recommend_Movie_System.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend_Movie_System.Services
{
    public class TrainingService : ITrainingService
    {
        private MLContext mContext = new MLContext();
        private static string trainingDataPath = Path.Combine(Environment.CurrentDirectory, "recommendation-ratings-train.csv");
        private static string testDataPath = Path.Combine(Environment.CurrentDirectory, "recommendation-ratings-test.csv");
        public void Train()
        {
            GetData();
             var pipeline = mContext.Transforms.Conversion.MapValueToKey(
                            inputColumnName: "userId",
                            outputColumnName: "userIdEncoded")
                            .Append(mContext.Transforms.Conversion.MapValueToKey(
                            inputColumnName: "movieId",
                            outputColumnName: "movieIdEncoded")

// step 2: find recommendations using matrix factorization
.Append(mContext.Recommendation().Trainers.MatrixFactorization(GetOptions())));
        }

        private void GetData()
        {
            mContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            mContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');
        }

        private MatrixFactorizationTrainer.Options GetOptions()
        {
            return new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };
        }

        
    }
}
