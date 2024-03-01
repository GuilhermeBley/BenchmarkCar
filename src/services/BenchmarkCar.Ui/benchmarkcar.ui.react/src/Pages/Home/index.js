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

    comparativeComponent() {
        return (
            <div class="container">
                <div class="row">
                  
                    <div class="col-md-6">
                        <div class="card mb-3">
                            <img src="https://via.placeholder.com/300" class="card-img-top" alt="Item A Image"/>
                                <div class="card-body">
                                    <h5 class="card-title">Item A</h5>
                                    <p class="card-text">Description of Item A.</p>
                                </div>
                        </div>
                    </div>
                  
                    <div class="col-md-6">
                        <div class="card mb-3">
                            <img src="https://via.placeholder.com/300" class="card-img-top" alt="Item B Image"/>
                                <div class="card-body">
                                    <h5 class="card-title">Item B</h5>
                                    <p class="card-text">Description of Item B.</p>
                                </div>
                        </div>
                    </div>
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
                    {this.comparativeComponent()}
                </div>
            </div>
        );
    }
}

export default withTranslation()(Home);