import React, { useState, type FC } from "react";
import {Modal,
    ModalOverlay,
    ModalContent,
    ModalHeader,
    ModalFooter,
    ModalBody,
    ModalCloseButton,} from '@chakra-ui/modal'
import { Button } from "@chakra-ui/react/button";
import { Input, useDisclosure } from "@chakra-ui/react";
import {
  FormControl,
  FormLabel,
  FormErrorMessage,
  FormHelperText,
  FormErrorIcon,
} from "@chakra-ui/form-control"
import { projectService } from "~/api/projectService";
import { customerService } from "~/api/customerService";
import type { CustomerCreateDTO } from "~/models/customer";
import type { ProjectCreateDTO, ProjectDTO } from "~/models/project";

interface IProps {
    project: ProjectDTO
    open: boolean
    onClose: () => void;
    onConfirm: (project: ProjectDTO) => void;
}

const EditProjectModal: FC<IProps> = ({project, open, onClose, onConfirm}) => {
  const [errors, setErrors] = useState<{ deadline?: string}>({});
  const formatDate = (date: string) => new Date(date).toISOString().split('T')[0];
  const [formData, setFormData] = useState({
    projectName: project.name,
    projectDeadline: formatDate(project.deadline),
    isCompleted: project.isCompleted
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

  const today = new Date().toISOString().split("T")[0];

  // Check if the project deadline is before today
  if (formData.projectDeadline < today || formData.projectDeadline === "") {
    setErrors({ deadline: "Choose a deadline that is not in the past" });
    return;
  }

    const updatedProject: ProjectDTO = {
      id: project.id,
      name: formData.projectName,
      deadline: formData.projectDeadline,
      freelancerId: "DEA192EE-B1D0-43DA-0A7D-08DD6C71F515",
      customerId: project.customerId,
      customerName: project.customerName,
      isCompleted: formData.isCompleted
    }
    await projectService.updateProject(updatedProject.id, updatedProject);
    onConfirm(updatedProject);
  };
  const handleClose = () => {
    onClose();
    setErrors({})
  }
  return (
    <>
      <Modal isOpen={open} onClose={handleClose} isCentered>
        <ModalOverlay
          bg="rgba(0, 0, 0, 0.1)" // Semi-transparent dark overlay
          backdropFilter="blur(2px)" // Blurs the background
        />
        <ModalContent mx='auto' my='300' width={600} backgroundColor='#ffffff' borderRadius={8} padding={10}>
          <ModalHeader mx='auto' mb={10}><strong>Edit project</strong> </ModalHeader>
          <ModalBody mx='auto'>
          <form onSubmit={handleSubmit} style={{ display: "flex" }}>
                {/* Project Form (Left Side) */}
                <div style={{ flex: 1, marginRight: "20px" }}>
                  <FormControl mb={4}>
                    <FormLabel htmlFor="projectName">Project Name</FormLabel>
                    <Input
                      required
                      id="projectName"
                      name="projectName"
                      value={formData.projectName}
                      onChange={handleInputChange}
                      placeholder="Enter project name"
                    />
                  </FormControl>

                  <FormControl mb={4} isInvalid={!!errors.deadline} isRequired>
                    <FormLabel htmlFor="projectDeadline">Project Deadline</FormLabel>
                    <Input
                      required
                      id="projectDeadline"
                      name="projectDeadline"
                      type="date"
                      value={formData.projectDeadline}
                      onChange={handleInputChange}
                    />
                    <FormErrorMessage textColor='red'>{errors.deadline}</FormErrorMessage>
                  </FormControl>
                </div>

                {/* Customer Form (Right Side) */}
                <div style={{ flex: 1 }}>
                  <FormControl mb={4} display="flex" alignItems="center">
                    <FormLabel htmlFor="isCompleted" mb="0">
                        Completed
                    </FormLabel>
                    <input
                        id="isCompleted"
                        name="isCompleted"
                        type="checkbox"
                        checked={formData.isCompleted}
                        onChange={(e) => setFormData({ ...formData, isCompleted: e.target.checked })}
                    />
                  </FormControl>
                </div>
              </form>
          </ModalBody>

          <ModalFooter mx='auto'>
              <Button bgColor={"green"} mr={3} type="submit" onClick={handleSubmit}>
                Update
              </Button>
              <Button bgColor={"red"} onClick={handleClose}>
                Close
              </Button>
            </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
};

export default EditProjectModal;
