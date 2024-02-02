using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;
using BenchmarkCar.Domain.Entities;
using System.Text.Json.Serialization;

namespace BenchmarkCar.Infrastructure.Model.CarApi;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class Make
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class MakeModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_id")]
    public int MakeId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("make")]
    public Make? Make { get; set; }
}

public class MakeModelTrimBody
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int MakeModelTrimId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("doors")]
    public int Doors { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("wheel_base")]
    public int WheelBase { get; set; }

    [JsonPropertyName("front_track")]
    public int FrontTrack { get; set; }

    [JsonPropertyName("rear_track")]
    public int RearTrack { get; set; }

    [JsonPropertyName("ground_clearance")]
    public int GroundClearance { get; set; }

    [JsonPropertyName("cargo_capacity")]
    public int CargoCapacity { get; set; }

    [JsonPropertyName("max_cargo_capacity")]
    public int MaxCargoCapacity { get; set; }

    [JsonPropertyName("curb_weight")]
    public int CurbWeight { get; set; }

    [JsonPropertyName("gross_weight")]
    public int GrossWeight { get; set; }

    [JsonPropertyName("max_payload")]
    public int MaxPayload { get; set; }

    [JsonPropertyName("max_towing_capacity")]
    public int MaxTowingCapacity { get; set; }

    public CreateBodyModel MapToCreateBodyModel()
        => new CreateBodyModel(
            ExternalId: Id.ToString(),
            Door: Doors,
            Length: Length,
            Seats: Seats,
            Width: Width);
}

public class MakeModelTrimEngine
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int MakeModelTrimId { get; set; }

    [JsonPropertyName("engine_type")]
    public string? EngineType { get; set; }

    [JsonPropertyName("fuel_type")]
    public string? FuelType { get; set; }

    [JsonPropertyName("cylinders")]
    public string? Cylinders { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("horsepower_hp")]
    public int HorsepowerHp { get; set; }

    [JsonPropertyName("horsepower_rpm")]
    public int HorsepowerRpm { get; set; }

    [JsonPropertyName("torque_ft_lbs")]
    public int TorqueFtLbs { get; set; }

    [JsonPropertyName("torque_rpm")]
    public int TorqueRpm { get; set; }

    [JsonPropertyName("valves")]
    public int Valves { get; set; }

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
            ExternalId: Id.ToString(),
            Valves: Valves,
            EngineSize: Size,
            HorsePowerHp: HorsepowerHp,
            HorsePowerRpm: HorsepowerRpm,
            TorqueFtLbs: TorqueFtLbs,
            TorqueRpm: TorqueRpm);
}

public class MakeModelTrimExteriorColor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int MakeModelTrimId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rgb")]
    public string? Rgb { get; set; }
}

public class MakeModelTrimInteriorColor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int MakeModelTrimId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rgb")]
    public string? Rgb { get; set; }
}

public class MakeModelTrimMileage
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_model_trim_id")]
    public int MakeModelTrimId { get; set; }

    [JsonPropertyName("fuel_tank_capacity")]
    public int FuelTankCapacity { get; set; }

    [JsonPropertyName("combined_mpg")]
    public int CombinedMpg { get; set; }

    [JsonPropertyName("epa_city_mpg")]
    public int EpaCityMpg { get; set; }

    [JsonPropertyName("epa_highway_mpg")]
    public int EpaHighwayMpg { get; set; }

    [JsonPropertyName("range_city")]
    public int RangeCity { get; set; }

    [JsonPropertyName("range_highway")]
    public int RangeHighway { get; set; }

    [JsonPropertyName("battery_capacity_electric")]
    public int BatteryCapacityElectric { get; set; }

    [JsonPropertyName("epa_time_to_charge_hr_240v_electric")]
    public int EpaTimeToChargeHr240vElectric { get; set; }

    [JsonPropertyName("epa_kwh_100_mi_electric")]
    public int EpaKwh100MiElectric { get; set; }

    [JsonPropertyName("range_electric")]
    public int RangeElectric { get; set; }

    [JsonPropertyName("epa_highway_mpg_electric")]
    public int EpaHighwayMpgElectric { get; set; }

    [JsonPropertyName("epa_city_mpg_electric")]
    public int EpaCityMpgElectric { get; set; }

    [JsonPropertyName("epa_combined_mpg_electric")]
    public int EpaCombinedMpgElectric { get; set; }
}

public class VehicleTrimResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("make_model_id")]
    public int MakeModelId { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("msrp")]
    public int Msrp { get; set; }

    [JsonPropertyName("invoice")]
    public int Invoice { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("modified")]
    public DateTime Modified { get; set; }

    [JsonPropertyName("make_model")]
    public MakeModel? MakeModel { get; set; }

    [JsonPropertyName("make_model_trim_body")]
    public MakeModelTrimBody? MakeModelTrimBody { get; set; }

    [JsonPropertyName("make_model_trim_engine")]
    public MakeModelTrimEngine? MakeModelTrimEngine { get; set; }

    [JsonPropertyName("make_model_trim_mileage")]
    public MakeModelTrimMileage? MakeModelTrimMileage { get; set; }

    [JsonPropertyName("make_model_trim_exterior_colors")]
    public List<MakeModelTrimExteriorColor> MakeModelTrimExteriorColors { get; set; }
        = new();

    [JsonPropertyName("make_model_trim_interior_colors")]
    public List<MakeModelTrimInteriorColor> MakeModelTrimInteriorColors { get; set; }
        = new();
}


