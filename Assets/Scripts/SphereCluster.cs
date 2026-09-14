using System.Collections.Generic;
using UnityEngine;

public class SphereCluster
{
    public struct PointData
    {
        public Vector3 LocalPosition; // Local position
        public float Radius;

        public PointData(Vector3 position, float radius)
        {
            LocalPosition = position;
            Radius = radius;
        }

        public Vector3 GetWorldPosition(Transform transform)
        {
            return transform.TransformPoint(LocalPosition);
        }
    }

    private readonly List<PointData> _points = new();

    public IReadOnlyList<PointData> Points => _points;

    public void Generate(
        int count,
        float clusterRadius,
        float minSphereRadius,
        float maxSphereRadius,
        float sizeBias,
        float distanceBias)
    {
        _points.Clear();

        const int maxAttempts = 100;

        // Large spheres first = easier packing
        List<float> radii = new();

        for (int i = 0; i < count; i++)
        {
            radii.Add(GetBiasedValue(
                minSphereRadius,
                maxSphereRadius,
                sizeBias));
        }

        radii.Sort((a, b) => b.CompareTo(a));

        foreach (float radius in radii)
        {
            bool placed = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector3 direction = Random.onUnitSphere;

                float distance = GetBiasedValue(
                    0f,
                    clusterRadius - radius,
                    distanceBias);

                Vector3 position = direction * distance;

                if (!Overlaps(position, radius))
                {
                    _points.Add(new PointData(position, radius));
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                Debug.LogWarning("Failed to place sphere.");
            }
        }
    }

    private bool Overlaps(Vector3 position, float radius)
    {
        foreach (var other in _points)
        {
            float requiredDistance = radius + other.Radius;

            if ((position - other.LocalPosition).sqrMagnitude <
                requiredDistance * requiredDistance)
            {
                return true;
            }
        }

        return false;
    }

    private float GetBiasedValue(float min, float max, float bias)
    {
        float t = Random.value;

        // 0 = mostly towards min
        // 0.5 = uniform
        // 1 = mostly towards max

        float exponent = Mathf.Lerp(3f, 0.333f, bias);

        t = Mathf.Pow(t, exponent);

        return Mathf.Lerp(min, max, t);
    }
}