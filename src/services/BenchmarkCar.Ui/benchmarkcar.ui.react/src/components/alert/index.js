import React, { useState } from 'react';
import './index.css'

/**
 * Top alerts messages
 * @param {Array} alerts 
 */
function topAlert(alerts, classCss = "alert-danger") {

    const [items, setItems] = useState(alerts);

    /**
     * @param {int} indexToRemove 
     */
    const removeArrayItem = (indexToRemove) => {
        const newItems = items.filter((item, index) => index !== indexToRemove);
        setItems(newItems);
    }

    let completeClassCss = "alert d-block " + classCss;

    let distinctArray = [...new Set(alerts)];

    return distinctArray.slice(0, 3).map((value, index) => (
        <div class={completeClassCss} role="alert">
            <div class="d-flex align-items-center">
                {value}
                <div class="ml-auto">
                    <button class="btn btn-outline-danger border-0" onClick={removeArrayItem(index)}>x</button>
                </div>
            </div>
        </div>
    ));
} 

export default topAlert;