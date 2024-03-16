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

    render() {

        const { t } = this.props;

        return (
            <div className="container">
                <div className="row">
                    <div className="col-md">
                        <div className="text-center">

                            <h2>{t('home-title')}</h2>

                            <div class="vehicleInput">
                                <input type="text" name="vehicleLeft" list="vehicleLeft" />
                                <datalist id="vehicleLeft">
                                    {this.state.makes.map((make) => (
                                        <option value={make.name}></option>
                                    ))}
                                </datalist>
                            </div>
                            <div class="vehicleInput">
                                <input type="text" name="vehicleRight" list="vehicleRight" />
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