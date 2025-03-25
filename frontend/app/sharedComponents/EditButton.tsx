import { Button } from "@chakra-ui/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPencil } from "@fortawesome/free-solid-svg-icons";

import React from "react";

interface EditButtonProps<T> {
  item: T;
  onEdit: (item: T) => void;
}

const EditButton = <T,>({ item, onEdit }: EditButtonProps<T>) => {
  return (
    <button
      onClick={() => onEdit(item)}
    >
      <FontAwesomeIcon icon={faPencil} />
      
    </button>
  );
};

export default EditButton;
