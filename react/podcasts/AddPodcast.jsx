import React from "react";
import Modal from "react-bootstrap/Modal";
import PodcastForm from "./PodcastForm";
import PropTypes from "prop-types";

function AddPodcast({ isModalShown, setShowModal, addNewPodcast }) {
  const handleClose = () => setShowModal(false);

  return (
    <React.Fragment>
      <Modal show={isModalShown} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Add a Podcast</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <PodcastForm onClose={handleClose} addNewPodcast={addNewPodcast} />
        </Modal.Body>
      </Modal>
    </React.Fragment>
  );
}

AddPodcast.propTypes = {
  isModalShown: PropTypes.bool.isRequired,
  setShowModal: PropTypes.func.isRequired,
  addNewPodcast: PropTypes.func.isRequired,
};

export default AddPodcast;
