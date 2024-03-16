import React, { Component } from 'react';
import { withTranslation } from 'react-i18next';
import axiosBenc from '../../api/axiosBenc'

import styles from './index.css';

class Home extends Component {
    constructor(props) {
        super(props);

        this.state = {
            left: null,
            error: null,
            makes: []
        }
    }

    componentDidMount() {
        document.title = "Home";
        this.setVehiclesMakes();
    }

    async setVehiclesMakes() {
        var response =
                await axiosBenc.get('/api/vehiclemake');

        if (response.status >= 200 && response.status < 300) {
            let data = response.data;
            this.setState(prevState =>
                prevState.makes = data
            );
            console.info('Data collected.' + data)
        }
    }

    /**
     * Only set the left vehicle
     * @param {string} vehicleLeftId 
     */
    handleInputRightChange(vehicleLeftId) {

        if (vehicleLeftId == null)
        {
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
        
        if (this.left == null)
        {
            console.log('Vehicle left is null.');
            return;
        }
        
        if (vehicleRightId == null)
        {
            console.log('Vehicle right is null.');
            return;
        }

        if (vehicleRightId == this.left)
        {
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
            <div className="container">
                <div className="row">
                    <div className="col-md">
                        <div className="text-center">

                            <h2>{t('home-title')}</h2>

                            <div class="vehicleInput">
                                <input type="text" name="vehicleLeft" list="vehicleLeft" onChange={(e) => this.handleInputLeftChange(e.target.value)}/>
                                <datalist id="vehicleLeft">
                                    {this.state.makes.map((make) => (
                                        <option value={make.name}></option>
                                    ))}
                                </datalist>
                            </div>
                            <div class="vehicleInput">
                                <input type="text" name="vehicleRight" list="vehicleRight" onChange={(e) => this.handleInputRightChange(e.target.value)}/>
                                <datalist id="vehicleRight">
                                    {this.state.makes.map((make) => (
                                        <option value={make.name}></option>
                                    ))}
                                </datalist>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default withTranslation()(Home);