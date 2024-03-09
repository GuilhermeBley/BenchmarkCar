const express = require('express');
const path = require('path');

var app = express();

app.use(express.static(path.resolve(__dirname, './build')));

// Allows you to set port in the project properties.
app.set('port', process.env.PORT || 3000);

const port = app.get('port');

var server = app.listen(port, function() {
    console.log(`listening in ${port}`);
});

module.exports = app