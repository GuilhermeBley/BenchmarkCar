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
            <div className="container">
                <div className="row">
                    <div className="col-md">
                        <div className="text-center">

                            <h2>{t('home-title')}</h2>

                            <div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default withTranslation()(Home);