import React, { useState } from "react";
import PropTypes from "prop-types";
import toastr from "toastr";
import debug from "sabio-debug";
import podcastsService from "services/podcastsService";
import podcastFormSchema from "./podcastSchema";
import FileUpload from "components/files/FileUpload";

const _logger = debug.extend("PodcastForm");

function PodcastForm({ onClose, addNewPodcast }) {
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    url: "",
    coverImageUrl: "",
    createdBy: "",
  });

  const [errors, setErrors] = useState({});

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevFormData) => ({
      ...prevFormData,
      [name]: value,
    }));
  };

  const validateForm = async () => {
    try {
      await podcastFormSchema.validate(formData, { abortEarly: false });
      return true;
    } catch (validationErrors) {
      const errors = {};
      validationErrors.inner.forEach((error) => {
        errors[error.path] = error.message;
      });
      setErrors(errors);
      return false;
    }
  };

  const addPodcast = async (e) => {
    e.preventDefault();

    const isValid = await validateForm();

    if (isValid) {
      _logger(formData);
      try {
        const response = await podcastsService.addPodcast(formData);
        onAddPodcastSuccess(response);
        onClose();
      } catch (error) {
        onAddPodcastError(error);
        toastr.error("Error adding podcast", error.message);
      }
    } else {
      toastr.error("Form has validation errors");
    }
  };

  const handleFileUploadComplete = (response) => {
    setFormData((prevFormData) => ({
      ...prevFormData,
      url: response.items[0].url,
    }));
    _logger(response.items[0].url);
  };

  const handleFileUploadError = (error) => {
    toastr.error("Error uploading file", error);
    _logger("Error uploading file", error);
  };

  const onAddPodcastSuccess = (response) => {
    _logger("Podcast added successfully", response);
    toastr.success("Podcast added successfully");
    addNewPodcast(formData);
  };

  const onAddPodcastError = (error) => {
    _logger("Error adding podcast", error);
  };

  return (
    <div>
      <FileUpload
        className="col-12"
        uploadComplete={handleFileUploadComplete}
        uploadError={handleFileUploadError}
      />

      <form onSubmit={addPodcast}>
        <div className="form-group">
          <label htmlFor="title" className="podc-form-lbl">
            Title
          </label>
          <input
            type="text"
            className="form-control"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleChange}
          />
          {errors.title && <div className="text-danger">{errors.title}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="description" className="podc-form-lbl">
            Description
          </label>
          <textarea
            className="form-control"
            id="description"
            name="description"
            value={formData.description}
            onChange={handleChange}
          ></textarea>
          {errors.description && (
            <div className="text-danger">{errors.description}</div>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="coverImageUrl" className="podc-form-lbl">
            Cover Image URL
          </label>
          <input
            type="url"
            className="form-control"
            id="coverImageUrl"
            name="coverImageUrl"
            value={formData.coverImageUrl}
            onChange={handleChange}
          />
          {errors.coverImageUrl && (
            <div className="text-danger">{errors.coverImageUrl}</div>
          )}
        </div>

        <button type="submit" className="podc-submit-btn-primary">
          Submit Podcast
        </button>
      </form>
    </div>
  );
}

PodcastForm.propTypes = {
  onClose: PropTypes.func.isRequired,
  addNewPodcast: PropTypes.func.isRequired,
};

export default PodcastForm;
