import './App.css';
import axios from 'axios';
import { useEffect, useState } from 'react';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import Dictionary from './dictionary';

function App() {
  const [data, setData] = useState([])

  // Loads all extension methods, like String.capitalize()
  //loadExtensionMethods()

  // Requests all rootwords and sets it to data state
  async function getData() {
    const requests = [];

    // Gets Rootwords with ID 12 to 21 and pushes it to request array if content is not empty
    for (let i = 12; i <= 21; i++) {
      let r = await axios.get(`http://localhost:5000/api/rootwords/${i}`)
      if (r.data) requests.push(r)
    }

    // Gets all data found in request array and sets it in data state once Promises are resolved
    let _data = await Promise.all(requests.map(r => r.data))
    setData(_data)
  }

  // Loads all data only one time
  useEffect(() => {
    getData()
  }, [])
  
  return (
    <div className="App">
      {/*<header className="App-header">
        <dl>
        { 
          data.map(d => {
            let rootword = d.rootword1.capitalize()
            return(
            <>
            <dt key={"word" + d.rootId}>{rootword}</dt>
            <dd key={"data" + d.rootId}>
              Meaning: {d.meaning}<br/>
              Pronunciation: {d.pronunciation}
            </dd>
            <br/>
            </>)
          })
        }
        </dl>
      </header>*/}
      <main className='App-main'>
        <BrowserRouter>
        <>
          <Routes>
            <Route path='/' element={<Dictionary/>}></Route>
            <Route path='/index' element={<Navigate to="/"/>}></Route>
          </Routes>
        </>
        </BrowserRouter>
      </main>
    </div>
  );
}

export default App;
