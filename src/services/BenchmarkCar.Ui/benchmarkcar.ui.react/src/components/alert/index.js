import React from 'react';
import './index.css'

/**
 * Top alerts messages
 * @param {Array} alerts 
 */
const topAlert = (alerts, classCss = "alert-danger") => {

    let completeClassCss = "alert d-block " + classCss;

    return alerts.map((value, index) => (
        <div class={completeClassCss} role="alert">
            {value}
        </div>
    ));
} 

export default topAlert;