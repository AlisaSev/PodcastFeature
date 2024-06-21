import React, { useEffect, useState } from "react";
import debug from "sabio-debug";
import podcastsService from "services/podcastsService";
import PodcastCard from "./PodcastCard";
import "./podcastslist.css";
import { Row, Col } from "react-bootstrap";
import PodcastPlayer from "./PodcastPlayer";
import AddPodcast from "./AddPodcast";

const _logger = debug.extend("PodcastsList");

function PodcastsList() {
  const [podcastsState, setPodcastsState] = useState({
    list: [],
    components: [],
    currentPage: 1,
    totalPages: 1,
  });

  const [selectedPodcast, setSelectedPodcast] = useState(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    getPodcasts(podcastsState.currentPage);
  }, [podcastsState.currentPage]);

  const getPodcasts = (page) => {
    const index = page - 1;
    podcastsService
      .getAll(index, 10)
      .then(onGetPodcastsSuccess)
      .catch(onGetPodcastsError);
  };

  const onGetPodcastsSuccess = (data) => {
    _logger(data);
    const podcasts = data.item.pagedItems;
    const totalItems = data.item.totalCount;
    const totalPages = Math.ceil(totalItems / 10);

    setPodcastsState((prevState) => ({
      ...prevState,
      list: podcasts,
      components: podcasts.map(mapPodcasts),
      totalPages: totalPages,
    }));
  };

  const onGetPodcastsError = (error) => {
    _logger("Error getting podcasts", error);
  };

  const prevPage = () => {
    if (podcastsState.currentPage > 1) {
      setPodcastsState((prevState) => ({
        ...prevState,
        currentPage: prevState.currentPage - 1,
      }));
    }
  };

  const nextPage = () => {
    if (podcastsState.currentPage < podcastsState.totalPages) {
      setPodcastsState((prevState) => ({
        ...prevState,
        currentPage: prevState.currentPage + 1,
      }));
    }
  };

  const handleAddPodcastButtonClick = () => {
    setShowModal(true);
  };

  const onListen = (podcast) => {
    _logger("Clicked podcast -->", podcast);
    setSelectedPodcast(podcast);
  };

  const mapPodcasts = (aPodcast) => {
    const createdBy = aPodcast.createdBy
      ? `${aPodcast.createdBy.firstName} ${aPodcast.createdBy.lastName}`
      : "";

    return (
      <Col key={aPodcast.id} xs={12} md={4} className="podc-col">
        <PodcastCard
          podcast={aPodcast}
          coverImageUrl={aPodcast.coverImageUrl || ""}
          createdBy={createdBy}
          onListen={() => onListen(aPodcast)}
        />
      </Col>
    );
  };

  const addNewPodcast = (newPodcast) => {
    setPodcastsState((prevState) => ({
      ...prevState,
      list: [newPodcast, ...prevState.list],
      components: [mapPodcasts(newPodcast), ...prevState.components],
    }));
    _logger("New Podcast Added", newPodcast);
  };

  return (
    <div className="main-podc-cards-container">
      <section className="podc-cards-header">
        <h1 className="podc-heading">Podcasts</h1>
        <p className="podc-cards-header-paragraph">
          Listen to the finest podcasts from the events world
        </p>
        <button
          className="podc-btn-add-new"
          onClick={handleAddPodcastButtonClick}
        >
          Add New Podcast
        </button>
      </section>
      <div className="podc-cards-container">
        <Row>{podcastsState.components}</Row>
      </div>
      <section className="podc-audio-player">
        {selectedPodcast && (
          <PodcastPlayer
            url={selectedPodcast.url}
            selectedPodcast={selectedPodcast}
            onPlay={() => setSelectedPodcast(null)}
          />
        )}
      </section>
      <div className="podc-pagination text-center">
        <button
          onClick={prevPage}
          className="podc-pagination-btn btn btn-primary"
          disabled={podcastsState.currentPage === 1}
        >
          Previous
        </button>
        <span className="pages-number">
          {podcastsState.currentPage} of {podcastsState.totalPages}
        </span>
        <button
          onClick={nextPage}
          className="podc-pagination-btn btn btn-primary"
          disabled={podcastsState.currentPage === podcastsState.totalPages}
        >
          Next
        </button>
      </div>

      <AddPodcast
        isModalShown={showModal}
        setShowModal={setShowModal}
        addNewPodcast={addNewPodcast}
      />
    </div>
  );
}

export default PodcastsList;
