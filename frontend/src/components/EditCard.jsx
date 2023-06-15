import classes from '../css/EditCard.module.css';

const EditCard = props => {
  return <section className={classes.container}>{props.children}</section>;
};

export default EditCard;
