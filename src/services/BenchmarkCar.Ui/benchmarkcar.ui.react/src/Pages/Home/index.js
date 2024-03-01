import React, { Component } from 'react';
import { withTranslation } from 'react-i18next';

import styles from './index.css';

class Home extends Component {
    constructor(props) {
        super(props);

        
    }

    componentDidMount() {
        document.title = "Home";
    }

    cardVehicleComponent(vehicle) {

        if (vehicle == null)
            return (
                <div class="col-md-6">
                    <div class="card mb-3">
                        <img src="https://via.placeholder.com/300" class="card-img-top" alt="Item A Image" />
                        <div class="card-body">
                            <span>Selecione um veículo</span>
                        </div>
                    </div>
                </div>
            );

        return (
            <div class="col-md-6">
                <div class="card mb-3">
                    <img src="https://via.placeholder.com/300" class="card-img-top" alt="Item A Image" />
                    <div class="card-body">
                        <h5 class="card-title">{vehicle.name}</h5>
                        <p class="card-text">{vehicle.description}</p>
                    </div>
                </div>
            </div>
        );
    }

    comparativeComponent(left, right) {

        return (
            <div class="container">
                <div class="row">

                    {this.cardVehicleComponent(left)}

                    {this.cardVehicleComponent(right)}

                </div>
            </div>
        );
    }

    render() {

        const { t } = this.props;

        return (
            <div>
                <h1>{t('home-title')}</h1>

                <div>
                    {this.comparativeComponent(null, null)}
                </div>
            </div>
        );
    }
}

class VehicleComparative {
    constructor(name, description, imgUrl) {
        this.name = name;
        this.description = description;
        this.imgUrl = imgUrl;
    }
}

export default withTranslation()(Home);