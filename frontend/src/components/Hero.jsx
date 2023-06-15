import classes from '../css/Hero.module.css';
import logo from '../images/logoShort.png';

const Hero = () => {
  return (
    <header className={classes.header}>
      <div>
        <img className={classes.headerLogo} src={logo} alt="logo" />
      </div>
      <div className={classes.headerTitle}>
        <h1>FLEET MANAGEMENT</h1>
        <h2>Klik op een knop om te starten</h2>
      </div>
    </header>
  );
};
export default Hero;
