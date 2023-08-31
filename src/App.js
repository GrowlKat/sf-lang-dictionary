import './App.css';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import Dictionary from './dictionary';

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
    </div>
  );
}

export default App;
