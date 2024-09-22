import './App.css';
import UnitList from './components/UnitList';
import { sync } from './services/UnitService';
import SyncForm from './components/SyncForm';

function App() {
  return (
    <div className="App">
      <header>
        <SyncForm></SyncForm>
        <UnitList/>
      </header>
    </div>
  );
}

export default App;
