var express = require('express');

var indexRoutes = require('./src/index');

var app = express();

app.use('/', indexRoutes);

// Allows you to set port in the project properties.
app.set('port', process.env.PORT || 3000);

var server = app.listen(app.get('port'), function() {
    console.log('listening');
});

module.exports = app