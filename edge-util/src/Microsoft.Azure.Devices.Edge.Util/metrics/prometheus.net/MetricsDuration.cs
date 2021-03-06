// Copyright (c) Microsoft. All rights reserved.
namespace Microsoft.Azure.Devices.Edge.Util.Metrics.Prometheus.Net
{
    using System.Collections.Generic;
    using global::Prometheus;

    public class MetricsDuration : BaseMetric, IMetricsDuration
    {
        readonly Summary summary;

        public MetricsDuration(string name, string description, List<string> labelNames, List<string> defaultLabelValues)
            : base(labelNames, defaultLabelValues)
        {
            this.summary = Metrics.CreateSummary(
                $"{name}_seconds",
                description,
                new SummaryConfiguration
                {
                    Objectives = new[]
                    {
                        new QuantileEpsilonPair(0.1, 0.01),
                        new QuantileEpsilonPair(0.5, 0.01),
                        new QuantileEpsilonPair(0.9, 0.01),
                        new QuantileEpsilonPair(0.99, 0.01),
                    },
                    LabelNames = labelNames.ToArray()
                });
        }

        public void Set(double value, string[] labelValues) =>
            this.summary
                .WithLabels(this.GetLabelValues(labelValues))
                .Observe(value);
    }
}
