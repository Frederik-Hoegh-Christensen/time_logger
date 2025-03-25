import {
    Modal,
    ModalOverlay,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
  } from "@chakra-ui/modal";
  import { Button } from "@chakra-ui/react";
  import type { FC } from "react";
  
  interface ActionConfirmationDialogProps {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => void;
    title?: string;
    message?: string;
  }
  
  const ActionConfirmationDialog: FC<ActionConfirmationDialogProps> = ({
    isOpen,
    onClose,
    onConfirm,
    title = "Confirm Action",
    message = "",
  }) => {
    return (
      <Modal isOpen={isOpen} onClose={onClose} isCentered={true}>
        <ModalOverlay bg="rgba(0, 0, 0, 0.1)"
          backdropFilter="blur(2px)" />
        <ModalContent 
          mx="auto" my="auto" width={600} backgroundColor="#ffffff" borderRadius={8} padding={10} display={"flex"}
        >
          <ModalHeader p={10} fontWeight='bold'>{title}</ModalHeader>
          <ModalBody>{message}</ModalBody>
          <ModalFooter>
            <Button onClick={onClose} mr={3}>
              Cancel
            </Button>
            <Button backgroundColor="blue" onClick={onConfirm}>
              Confirm
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    );
  };
  
  export default ActionConfirmationDialog;
  