/**
 * @param {VehicleComparative} vehicle
 */
const CardVehicleComponent = (vehicle) => {

    return (
        <div class="col-md-6">
            <div class="card mb-3">
                <img src={vehicle.imgUrl} class="card-img-top" alt="Item A Image" />
                <div class="card-body">
                    <h5 class="card-title">{vehicle.name}</h5>
                    <p class="card-text">{vehicle.description}</p>
                </div>
            </div>
        </div>
    );
}

class VehicleComparative {
    constructor(name, description, imgUrl) {
        this.name = name;
        this.description = description;
        this.imgUrl = imgUrl ?? "https://via.placeholder.com/300";
    }
}

export default CardVehicleComponent;