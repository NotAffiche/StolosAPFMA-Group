import classes from '../css/ErrorPageContent.module.css';

function ErrorPageContent({ title, children }) {
  return (
    <div className={classes.content}>
      <h1>{title}</h1>
      {children}
    </div>
  );
}

export default ErrorPageContent;
