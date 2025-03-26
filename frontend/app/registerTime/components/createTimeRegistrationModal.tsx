import React, { useEffect, useState } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
} from "@chakra-ui/modal";
import { Button } from "@chakra-ui/react";
import { Input, useDisclosure } from "@chakra-ui/react";
import { FormControl, FormErrorMessage, FormLabel } from "@chakra-ui/form-control";
import { timeRegistrationService } from "~/api/timeRegistrationService";
import type { TimeRegistrationCreateDTO } from "~/models/timeRegistration";
import { useTimeRegistrationContext } from "~/contexts/timeRegistrationContext";
import ProjectDropDown from "~/projectOverview/components/projectDropDown";
import { useProjectContext } from "~/contexts/projectContext";
import { convertToDecimal, convertToHHMM, validateTimeFormat } from "~/utils/timeHelper";
interface CreateTimeRegistrationModalProps {
  timeRegistration: TimeRegistrationCreateDTO | undefined;
}
const CreateTimeRegistrationModal = ({timeRegistration} : CreateTimeRegistrationModalProps) => {
  const { open, onOpen, onClose } = useDisclosure();
  const {selectedDate, setSelectedDate} = useTimeRegistrationContext();
  const {projects, setSelectedProject, selectedProject} = useProjectContext();
  const [errors, setErrors] = useState<{ hoursWorked?: string, projectId?: string }>({});
  const [formData, setFormData] = useState({
    freelancerId: "DEA192EE-B1D0-43DA-0A7D-08DD6C71F515", // Static for now
    hoursWorked: "",
    description: "",
    projectId: ""
  });
 
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!selectedProject) {
      setErrors({ projectId: "Please choose a project" });
      return; 
    }
    // Validate hoursWorked
    if (!validateTimeFormat(formData.hoursWorked)) {
      setErrors({ hoursWorked: "Enter time in HH:MM format (00:00 - 23:59)" });
      return; 
    }
    
      setErrors({});
  
    const timeRegistration: TimeRegistrationCreateDTO = {
      projectId: selectedProject!.id,
      freelancerId: formData.freelancerId,
      workDate: selectedDate!.toISOString().split("T")[0],
      hoursWorked: convertToDecimal(formData.hoursWorked),
      description: formData.description,
    };
  
    await timeRegistrationService.createTimeRegistration(timeRegistration);
    onClose();
    setSelectedDate(new Date(selectedDate!));
  };

  const handleClose = () => {
    onClose()
    setErrors({});
    setFormData({
      freelancerId: "DEA192EE-B1D0-43DA-0A7D-08DD6C71F515", // Static for now
      hoursWorked: "",
      description: "",
      projectId: ""
    })
  }

  return (
    <>
      <Button style={{ marginLeft: 10 }} onClick={onOpen}>
        Register Time
      </Button>
      <Modal isOpen={open} onClose={handleClose} isCentered>
        <ModalOverlay
          bg="rgba(0, 0, 0, 0.1)"
          backdropFilter="blur(2px)"
        />
        <ModalContent mx="auto" my="auto" width={600} backgroundColor="#ffffff" borderRadius={8} padding={10}>
          <ModalHeader mx="auto" mb={10}>
            <strong>Register Time</strong>
          </ModalHeader>
          <ModalBody mx="auto">
            <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column" }}>
              <FormControl mb={4} isInvalid={!!errors.projectId} isRequired>
                <FormLabel htmlFor="projectId">Project</FormLabel>
                <ProjectDropDown 
                    projects={projects} 
                    selectedProject={selectedProject} 
                    onProjectSelect={setSelectedProject} 
                />
                <FormErrorMessage textColor="red">{errors.projectId}</FormErrorMessage>
              </FormControl>

              <FormControl mb={4} isInvalid={!!errors.hoursWorked} isRequired>
                <FormLabel htmlFor="hoursWorked">Hours Worked</FormLabel>
                <Input
                  id="hoursWorked"
                  name="hoursWorked"
                  type="text"
                  value={formData.hoursWorked}
                  onChange={handleInputChange} // Custom handler
                  placeholder="00:00"
                  maxLength={5}
                />
                <FormErrorMessage textColor='red'>{errors.hoursWorked}</FormErrorMessage>
              </FormControl>

              <FormControl mb={4}>
                <FormLabel htmlFor="description">Description</FormLabel>
                <Input
                  required
                  id="description"
                  name="description"
                  value={formData.description}
                  onChange={handleInputChange}
                  placeholder="Enter work description"
                />
              </FormControl>
            </form>
          </ModalBody>

          <ModalFooter mx="auto">
            <Button bgColor={"green"} mr={3} type="submit" onClick={handleSubmit}>
              Submit
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

export default CreateTimeRegistrationModal;
