using System.Numerics;
using Shouldly;

namespace TestMode.UnitTests;

public static class ShouldlyExtensions
{
    public static void ShouldBe(this Vector3 actual, Vector3 expected)
    {
        actual.X.ShouldBe(expected.X, customMessage: $"should be (X) {expected} but was {actual}");
        actual.Y.ShouldBe(expected.Y, customMessage: $"should be (Y) {expected} but was {actual}");
        actual.Z.ShouldBe(expected.Z, customMessage: $"should be (Z) {expected} but was {actual}");
    }
}
