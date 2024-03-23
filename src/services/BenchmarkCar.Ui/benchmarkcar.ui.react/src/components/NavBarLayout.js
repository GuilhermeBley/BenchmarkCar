import React  from 'react';

import { useTranslation } from 'react-i18next';

const NavMenu = () => {

    const { t, i18n } = useTranslation();

    const HandleLanguageChange = (event) => {
        const selectedLanguage = event.target.value;
    
        i18n.changeLanguage(selectedLanguage);
    }

    return (
        <nav class="navbar navbar-expand-md navbar-light bg-primary">
            <a class="navbar-brand text-white" href="#">{t("nav-title")}</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                </ul>
                <form class="form-inline my-2 my-lg-0">
                    <select class="custom-select" onChange={HandleLanguageChange}>
                        <option value="en">en-US</option>
                        <option value="pt">pt-BR</option>
                    </select>
                </form>
            </div>
        </nav>
    );
}

const Layout = ({ children }) => {

    return (
        <div className='h-100'>
            <NavMenu />
            {children}
        </div>
    );

}

export default Layout;