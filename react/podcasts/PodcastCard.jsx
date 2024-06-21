import React, { useState } from "react";
import { FaPlay } from "react-icons/fa";
import PropTypes from "prop-types";

const PodcastCard = ({ podcast, coverImageUrl, createdBy, onListen }) => {
  const [isHovered, setIsHovered] = useState(false);

  const handleMouseEnter = () => {
    setIsHovered(true);
  };

  const handleMouseLeave = () => {
    setIsHovered(false);
  };

  const handleClick = () => {
    onListen(podcast);
  };

  return (
    <div className="podcast-card" onClick={handleClick}>
      <div
        className="podcast-image-container"
        onMouseEnter={handleMouseEnter}
        onMouseLeave={handleMouseLeave}
      >
        <img
          src={coverImageUrl}
          alt={podcast.title}
          className="podcast-image"
        />
        {isHovered && (
          <div className="play-icon-container">
            <FaPlay className="play-icon" />
          </div>
        )}
      </div>
      <div className="podcast-info">
        <h3>{podcast.title}</h3>
        <p className="podc-description">{podcast.description}</p>
        {createdBy && <p className="podc-createdby">Created by: {createdBy}</p>}
      </div>
    </div>
  );
};

PodcastCard.propTypes = {
  podcast: PropTypes.shape({
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    url: PropTypes.string.isRequired,
  }).isRequired,
  coverImageUrl: PropTypes.string.isRequired,
  createdBy: PropTypes.string,
  onListen: PropTypes.func.isRequired,
};

export default PodcastCard;
