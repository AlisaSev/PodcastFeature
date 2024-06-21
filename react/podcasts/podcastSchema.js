import * as Yup from "yup";

const podcastFormSchema = Yup.object().shape({
    title: Yup.string()
        .min(2, "Title must be at least 2 characters")
        .max(255, "Title must be less than 255 characters")
        .required("Title is required"),
    description: Yup.string()
        .min(2, "Description must be at least 2 characters")
        .max(4000, "Description must be less than 4000 characters")
        .required("Description is required"),
    coverImageUrl: Yup.string()
        .url("URL must be a valid URL")
        .required("URL is required"),
    });


    export default podcastFormSchema;
