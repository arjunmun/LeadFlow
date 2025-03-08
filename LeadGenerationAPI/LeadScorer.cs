using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;

public class LeadScorer
{
    private MLContext mlContext = new();

    public class LeadData
    {
        [LoadColumn(0)] public int IndustryMatch;
        [LoadColumn(1)] public int JobTitleMatch;
        [LoadColumn(2)] public int Connections;
        [LoadColumn(3), ColumnName("Label")] public bool SuccessfulLead;
    }

    public float ScoreLead(LeadData lead)
    {
        var data = new List<LeadData> {
            new() { IndustryMatch = 1, JobTitleMatch = 1, Connections = 3, SuccessfulLead = true },
            new() { IndustryMatch = 0, JobTitleMatch = 1, Connections = 2, SuccessfulLead = false }
        };

        var trainingData = mlContext.Data.LoadFromEnumerable(data);
        var pipeline = mlContext.Transforms.Concatenate("Features", "IndustryMatch", "JobTitleMatch", "Connections")
                        .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        var model = pipeline.Fit(trainingData);
        var predictionEngine = mlContext.Model.CreatePredictionEngine<LeadData, LeadPrediction>(model);
        var prediction = predictionEngine.Predict(lead);
        return prediction.Probability * 100;
    }

    public class LeadPrediction { [ColumnName("Score")] public float Probability; }
}
