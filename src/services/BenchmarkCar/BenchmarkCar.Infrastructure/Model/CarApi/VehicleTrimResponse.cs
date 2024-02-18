using BenchmarkCar.Application.Commands.CreateVehicleModelDetails;
using BenchmarkCar.Domain.Entities;
using System.Text.Json.Serialization;

namespace BenchmarkCar.Infrastructure.Model.CarApi;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class Make
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class MakeModel
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_id")]
    public int? MakeId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("make")]
    public Make? Make { get; set; }
}

public class MakeModelTrimBody
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int? MakeModelTrimId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("doors")]
    public int? Doors { get; set; }

    [JsonPropertyName("length")]
    public string? Length { get; set; }

    [JsonPropertyName("width")]
    public string? Width { get; set; }

    [JsonPropertyName("seats")]
    public int? Seats { get; set; }

    [JsonPropertyName("height")]
    public string? Height { get; set; }

    [JsonPropertyName("wheel_base")]
    public string? WheelBase { get; set; }

    [JsonPropertyName("front_track")]
    public object? FrontTrack { get; set; }

    [JsonPropertyName("rear_track")]
    public object? RearTrack { get; set; }

    [JsonPropertyName("ground_clearance")]
    public object? GroundClearance { get; set; }

    [JsonPropertyName("cargo_capacity")]
    public string? CargoCapacity { get; set; }

    [JsonPropertyName("max_cargo_capacity")]
    public object? MaxCargoCapacity { get; set; }

    [JsonPropertyName("curb_weight")]
    public int? CurbWeight { get; set; }

    [JsonPropertyName("gross_weight")]
    public object? GrossWeight { get; set; }

    [JsonPropertyName("max_payload")]
    public object? MaxPayload { get; set; }

    [JsonPropertyName("max_towing_capacity")]
    public object? MaxTowingCapacity { get; set; }

    public CreateBodyModel MapToCreateBodyModel()
        => new CreateBodyModel(
            ExternalId: Id.ToString() ?? string.Empty,
            Door: Doors ?? 0,
            Length: Convert.ToDecimal(Length),
            Seats: Seats ?? 0,
            Width: Convert.ToDecimal(Width));
}

public class MakeModelTrimEngine
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int?  MakeModelTrimId { get; set; }

    [JsonPropertyName("engine_type")]
    public string? EngineType { get; set; }

    [JsonPropertyName("fuel_type")]
    public string? FuelType { get; set; }

    [JsonPropertyName("cylinders")]
    public string? Cylinders { get; set; }

    [JsonPropertyName("size")]
    public string? Size { get; set; }

    [JsonPropertyName("horsepower_hp")]
    public int? HorsepowerHp { get; set; }

    [JsonPropertyName("horsepower_rpm")]
    public int? HorsepowerRpm { get; set; }

    [JsonPropertyName("torque_ft_lbs")]
    public int? TorqueFtLbs { get; set; }

    [JsonPropertyName("torque_rpm")]
    public int? TorqueRpm { get; set; }

    [JsonPropertyName("valves")]
    public int? Valves { get; set; }

    [JsonPropertyName("valve_timing")]
    public string? ValveTiming { get; set; }

    [JsonPropertyName("cam_type")]
    public string? CamType { get; set; }

    [JsonPropertyName("drive_type")]
    public string? DriveType { get; set; }

    [JsonPropertyName("transmission")]
    public string? Transmission { get; set; }

    public CreateEngineModel MapToCreateEngineModel()
        => new CreateEngineModel(
            ExternalId: Id.ToString() ?? string.Empty,
            Valves: Valves,
            EngineSize: (Convert.ToDouble(Size) as int?) ?? 0,
            HorsePowerHp: HorsepowerHp,
            HorsePowerRpm: HorsepowerRpm,
            TorqueFtLbs: TorqueFtLbs,
            TorqueRpm: TorqueRpm);
}

public class MakeModelTrimExteriorColor
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int? MakeModelTrimId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rgb")]
    public string? Rgb { get; set; }
}

public class MakeModelTrimInteriorColor
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int? MakeModelTrimId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rgb")]
    public string? Rgb { get; set; }
}

public class MakeModelTrimMileage
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int? MakeModelTrimId { get; set; }

    [JsonPropertyName("fuel_tank_capacity")]
    public string? FuelTankCapacity { get; set; }

    [JsonPropertyName("combined_mpg")]
    public int? CombinedMpg { get; set; }

    [JsonPropertyName("epa_city_mpg")]
    public int? EpaCityMpg { get; set; }

    [JsonPropertyName("epa_highway_mpg")]
    public int? EpaHighwayMpg { get; set; }

    [JsonPropertyName("range_city")]
    public int? RangeCity { get; set; }

    [JsonPropertyName("range_highway")]
    public int? RangeHighway { get; set; }

    [JsonPropertyName("battery_capacity_electric")]
    public object? BatteryCapacityElectric { get; set; }

    [JsonPropertyName("epa_time_to_charge_hr_240v_electric")]
    public object? EpaTimeToChargeHr240vElectric { get; set; }

    [JsonPropertyName("epa_kwh_100_mi_electric")]
    public object? EpaKwh100MiElectric { get; set; }

    [JsonPropertyName("range_electric")]
    public object? RangeElectric { get; set; }

    [JsonPropertyName("epa_highway_mpg_electric")]
    public object? EpaHighwayMpgElectric { get; set; }

    [JsonPropertyName("epa_city_mpg_electric")]
    public object? EpaCityMpgElectric { get; set; }

    [JsonPropertyName("epa_combined_mpg_electric")]
    public object? EpaCombinedMpgElectric { get; set; }
}

public class VehicleTrimResponse
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_id")]
    public int? MakeModelId { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("msrp")]
    public int? Msrp { get; set; }

    [JsonPropertyName("invoice")]
    public int? Invoice { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("modified")]
    public DateTime? Modified { get; set; }

    [JsonPropertyName("make_model_trim_interior_colors")]
    public List<MakeModelTrimInteriorColor> MakeModelTrimInteriorColors { get; set; }
        = new();

    [JsonPropertyName("make_model_trim_exterior_colors")]
    public List<MakeModelTrimExteriorColor> MakeModelTrimExteriorColors { get; set; }
        = new();

    [JsonPropertyName("make_model_trim_mileage")]
    public MakeModelTrimMileage? MakeModelTrimMileage { get; set; }

    [JsonPropertyName("make_model_trim_engine")]
    public MakeModelTrimEngine? MakeModelTrimEngine { get; set; }

    [JsonPropertyName("make_model_trim_body")]
    public MakeModelTrimBody? MakeModelTrimBody { get; set; }

    [JsonPropertyName("make_model")]
    public MakeModel? MakeModel { get; set; }
}


