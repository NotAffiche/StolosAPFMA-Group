import '../css/StartScreen.css';
import Hero from '../components/Hero';
import Buttons from '../components/Buttons.jsx';

const StartPage = props => {
  return (
    <div className="startscreen">
      <Hero />
      <Buttons />
    </div>
  );
};

export default StartPage;
