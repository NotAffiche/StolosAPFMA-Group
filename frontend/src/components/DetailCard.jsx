import { useNavigate, useSubmit } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faCircleArrowLeft,
  faTrashCan,
  faPen,
} from '@fortawesome/free-solid-svg-icons';
import classes from '../css/DetailCard.module.css';

const DetailCard = props => {
  const navigate = useNavigate();
  const submit = useSubmit();
  function startDeleteHandler() {
    const proceed = window.confirm('Bent u zeker dat u wil doorgaan?');

    if (proceed) {
      submit(null, { method: 'delete' });
    }
  }

  return (
    <section className={classes.container}>
      {props.children}
      <div className={classes.buttons}>
        <button className={classes.back} onClick={() => navigate(-1)}>
          <FontAwesomeIcon icon={faCircleArrowLeft} /> Terug
        </button>
        <button
          onClick={() => navigate(`/${props.type}s/${props.id}/edit`)}
          className={classes.edit}
        >
          <FontAwesomeIcon icon={faPen} /> Bewerken
        </button>
        <button onClick={startDeleteHandler} className={classes.delete}>
          <FontAwesomeIcon icon={faTrashCan} /> Verwijderen
        </button>
      </div>
    </section>
  );
};

export default DetailCard;
