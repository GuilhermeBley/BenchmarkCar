import React, {Component} from 'react';
import { withTranslation } from 'react-i18next';

class Home extends Component {
    constructor(props) {
        super(props);
      }

    componentDidMount() {
        document.title = "Home";
    }

    render() {

        const { t } = this.props;

        return (
            <div>
                <h1>{t('home-title')}</h1>

                
            </div>
        );
    }    
}

export default withTranslation()(Home);