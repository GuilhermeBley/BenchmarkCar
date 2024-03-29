import React, { Component } from 'react';
import { withTranslation } from 'react-i18next';
import axiosBenc from '../../api/axiosBenc'
import withRouter from '../../components/withRouter'

import './index.css';

class Home extends Component {
    constructor(props) {
        super(props);

        this.state = {
            left: null,
            errors: [],
            makes: []
        }

        this.handleSubmitComparison = this.handleSubmitComparison.bind(this);
    }

    componentDidMount() {
        document.title = "Home";
        this.setVehiclesMakes();
    }

    async setVehiclesMakes() {

        try {
            var response =
                await axiosBenc.get('/api/vehiclemodel/make-model');

            if (response.status >= 200 && response.status < 300) {
                let data = response.data;
                this.setState(prevState =>
                    prevState.makes = data
                );
                console.info('Data collected.' + data)
            }
        }
        catch
        {
            console.error("Failed to get vehicles.");
        }
    }

    handleSubmitComparison = () => {
        if (this.state.left == null || this.state.right == null)
        {
            this.handleError(t('Vehicle-not-selected', 'Please, select the vehicles to compare.'));
            return;
        }

        var url
            = 'compare-vehicles/{left}_{right}'
                .replace('{left}', this.state.left)
                .replace('{right}', this.state.right);
        this.props.navigate(url);
    }

    handleError(errorMessage) {
        this.setState(prevState =>
            prevState.errors.push(errorMessage));
    }

    /**
     * Only set the left vehicle
     * @param {string} vehicleLeftId 
     */
    handleInputRightChange(vehicleLeftId) {

        if (vehicleLeftId == null) {
            console.log('Vehicle left is null.');
            return;
        }

        this.setState(prevState => {
            prevState.left = vehicleLeftId
        });
    }

    /**
     * Redirect user to vehicle comparison page
     * @param {string} vehicleRightId 
     */
    handleInputLeftChange(vehicleRightId) {

        const { t } = this.props;

        if (this.left == null) {
            console.log('Vehicle left is null.');
            return;
        }

        if (vehicleRightId == null) {
            console.log('Vehicle right is null.');
            return;
        }

        if (vehicleRightId === this.left) {
            this.setState(prevState => {
                prevState.error = t('sameVehicleInputError', '')
            });
            return;
        }

        console.log("Redirecting to another page.");
    }

    render() {

        const { t } = this.props;

        return (
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md h-100">
                        <div class="card h-100 border-0 justify-content-center text-center">

                            <h2 class="mb-5">{t('home-title')}</h2>

                            <div class="vehicleInput my-1  w-vehicleInput">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text" id="vehicleLeft">
                                            <i class="bi bi-car-front"></i>
                                        </span>
                                    </div>
                                    <input class="form-control" placeholder="Type the vehicle model or make" aria-label="Type the vehicle model or make" type="text" name="vehicleLeft" list="vehicleLeftList" onChange={(e) => this.handleInputRightChange(e.target.value)} />
                                    <datalist id="vehicleLeftList">
                                        {this.state.makes.map((make) => (
                                            <option value={make.entireName}></option>
                                        ))}
                                    </datalist>
                                </div>
                            </div>

                            <div class="vehicleInput my-1  w-vehicleInput">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text" id="vehicleRight">
                                            <i class="bi bi-car-front-fill"></i>
                                        </span>
                                    </div>
                                    <input class="form-control" placeholder="Type the vehicle model or make" aria-label="Type the vehicle model or make" type="text" name="vehicleRight" list="vehicleRightList" onChange={(e) => this.handleInputRightChange(e.target.value)} />
                                    <datalist id="vehicleRightList">
                                        {this.state.makes.map((make) => (
                                            <option value={make.entireName}></option>
                                        ))}
                                    </datalist>
                                </div>
                            </div>
                            <div class="submit-vehicles-to-compare-area my-1">
                                <button class="btn btn-outline-primary" onClick={this.handleSubmitComparison}>
                                    {t('buttonSubmitVehiclesToCompare', 'make comparison')}
                                </button>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

const transalatedPage = withTranslation()(Home);

export default withRouter(transalatedPage);