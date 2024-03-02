import React, { Component } from 'react';
import { withTranslation } from 'react-i18next';
import axiosBenc from '../../api/axiosBenc'

import styles from './index.css';

class Home extends Component {
    constructor(props) {
        super(props);

        this.state = {
            left: null,
            right: null,
            makes: []
        }
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

    setVehiclesMakes() {
        let response = [];
        let success = false;

        axiosBenc
            .get('/api/vehiclemake')
            .then(response => {
                if (response.status >= 200 && response.status < 300) {
                    response = response.data;
                    success = true;
                }
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
        
        if (success)
            this.setState(prevState => 
                prevState.makes = response
            );
    }

    render() {

        const { t } = this.props;

        this.setVehiclesMakes();

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