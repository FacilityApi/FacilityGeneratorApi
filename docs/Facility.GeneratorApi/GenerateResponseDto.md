# GenerateResponseDto class

Response for Generate.

```csharp
public sealed class GenerateResponseDto : ServiceDto<GenerateResponseDto>
```

## Public Members

| name | description |
| --- | --- |
| [GenerateResponseDto](GenerateResponseDto/GenerateResponseDto.md)() | Creates an instance. |
| [Failure](GenerateResponseDto/Failure.md) { get; set; } | The failure, if any. |
| [Output](GenerateResponseDto/Output.md) { get; set; } | The output from the generator. |
| override [IsEquivalentTo](GenerateResponseDto/IsEquivalentTo.md)(…) | Determines if two DTOs are equivalent. |

## Protected Members

| name | description |
| --- | --- |
| override [JsonSerializer](GenerateResponseDto/JsonSerializer.md) { get; } | The JSON serializer. |

## See Also

* namespace [Facility.GeneratorApi](../Facility.GeneratorApi.md)
* [GenerateResponseDto.cs](https://github.com/FacilityApi/FacilityGeneratorApi/tree/master/src/Facility.GeneratorApi/GenerateResponseDto.cs)

<!-- DO NOT EDIT: generated by xmldocmd for Facility.GeneratorApi.dll -->
