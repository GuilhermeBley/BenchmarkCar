import axios from 'axios';


const createApiBench = () =>{

  const apiUrl = process.env.REACT_APP_BENCHMARKCAR_URL;
  
  console.log('Benchmark API: ' + apiUrl);

  return axios.create({
    baseURL: apiUrl,
    timeout: 50000,
    headers: {
      'Content-Type': 'application/json',
      },
    });
}

const axiosBenc = createApiBench();

export default axiosBenc;
