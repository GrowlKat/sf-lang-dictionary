import './App.css';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import Dictionary from './dictionary';
import { BsGithub, BsLinkedin, BsTwitter } from 'react-icons/bs'

function App() {
  return (
    <div className="App">
      <main className='App-main'>
        {/* Sets the routes and components to be rendered */}
        <BrowserRouter>
        <>
          <Routes>
            <Route path='/' element={<Dictionary/>}></Route>
            <Route path='/index' element={<Navigate to="/"/>}></Route>
          </Routes>
        </>
        </BrowserRouter>
      </main>
      
      <footer className='App-footer'>
        <ul>
          <li>Developed by&nbsp;<b>Growl Kat</b></li>
          <li>{<BsLinkedin/>}<a href='https://linkedin.com/in/growlkat'>/in/growlkat</a></li>
          <li>{<BsGithub/>}<a href='https://github.com/GrowlKat'>/GrowlKat</a></li>
          <li>{<BsTwitter/>}<a href='https://twitter.com/Growl_Kat'>@Growl_Kat</a></li>
        </ul>
      </footer>
    </div>
  );
}

export default App;
