namespace OliverScheer.OpenApiSample.Models;

public record BaseMathRequest
{
    public required int Value1 { get; init; }
    public required int Value2 { get; init; }
}

public record AddRequest : BaseMathRequest { }

public record SubtractRequest : BaseMathRequest { }

public record MultiplyRequest : BaseMathRequest { }

public record DivideRequest : BaseMathRequest { }

public record RandomValueRequest
{
    public int Min { get; init; } = 0;
    public int Max { get; init; } = 100;
}