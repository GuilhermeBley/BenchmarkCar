import axios from 'axios';

const apiUrl = process.env.BENCHMARKCAR_URL;

const axiosBenc = axios.create({
  baseURL: apiUrl,
  timeout: 5000,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default axiosBenc;
