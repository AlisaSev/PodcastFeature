import React, { useEffect } from "react";
import PodcastAudioPlayer from "react-h5-audio-player";
import "react-h5-audio-player/lib/styles.css";
import PropTypes from "prop-types";

const PodcastPlayer = (props) => {
  useEffect(() => {
    if (props.selectedPodcast) {
    }
  }, [props.selectedPodcast]);

  return <PodcastAudioPlayer src={props.url} />;
};

PodcastPlayer.propTypes = {
  url: PropTypes.string.isRequired,
  selectedPodcast: PropTypes.shape({
    id: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    url: PropTypes.string.isRequired,
    coverImageUrl: PropTypes.string.isRequired,
    createdBy: PropTypes.string.isRequired,
  }),
};

export default PodcastPlayer;
