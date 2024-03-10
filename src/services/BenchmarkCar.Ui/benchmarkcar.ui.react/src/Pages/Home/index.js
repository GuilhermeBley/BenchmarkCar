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

    async setVehiclesMakes() {
        var response =
                await axiosBenc.get('/api/vehiclemake');

        if (response.status >= 200 && response.status < 300) {
            let data = await response.json();
            this.setState(prevState =>
                prevState.makes = data
            );
            console.success('Data collected.' + data)
        }
    }

    render() {

        const { t } = this.props;

        this.setVehiclesMakes();

        return (
            <div className="container">
                <div className="row">
                    <div className="col-md">
                        <div className="text-center">

                            <h2>{t('home-title')}</h2>

                            <div>
                                <input type="text" name="city" list="cityname"/>
                                <datalist id="cityname">
                                    <option value="Boston"></option>
                                    {this.state.makes.map((make) => (
                                        <option value={make.Id}>{make.Name}</option>
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