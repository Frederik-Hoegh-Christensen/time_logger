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
import type { TimeRegistrationCreateDTO, TimeRegistrationDTO } from "~/models/timeRegistration";
import { useTimeRegistrationContext } from "~/contexts/timeRegistrationContext";
import ProjectDropDown from "~/projectOverview/components/projectDropDown";
import { useProjectContext } from "~/contexts/projectContext";
import { convertToDecimal, convertToHHMM, validateTimeFormat } from "~/utils/timeHelper";


const EditTimeRegistrationModal = () => {
  const {openEditTimeRegistrationModal, setOpenEditTimeRegistrationModal, selectedTimeRegistration} = useProjectContext();
  const {selectedDate, setSelectedDate} = useTimeRegistrationContext();
  const {projects, setProjects, setSelectedProject, selectedProject} = useProjectContext();
  const [errors, setErrors] = useState<{ hoursWorked?: string, projectId?: string }>({});
  const [formData, setFormData] = useState(() => {
    return {
      id: "",
      freelancerId: "5088F034-274E-412B-2394-08DD648C3E34",
      hoursWorked: "",
      description: "",
    };
  
    
  });

  //set selected project
  useEffect(() => {
    if (!selectedTimeRegistration) {
      console.log("selectedTimeRegistration is undefined");
      return;
    }
  
  
    const project = projects.find(p => p.id === selectedTimeRegistration.projectId);
    if (project) {
      setSelectedProject(project);
    }
  
    setFormData({
      id: selectedTimeRegistration.id,
      freelancerId: "5088F034-274E-412B-2394-08DD648C3E34",
      hoursWorked: convertToHHMM(selectedTimeRegistration.hoursWorked),
      description: selectedTimeRegistration.description ?? "",
    });
  }, [selectedTimeRegistration]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
  
    if (!selectedTimeRegistration) {
      console.error("selectedTimeRegistration is undefined when submitting.");
      return;
    }
  
    if (!selectedProject) {
      setErrors({ projectId: "Please choose a project" });
      return;
    }
  
    if (!validateTimeFormat(formData.hoursWorked)) {
      setErrors({ hoursWorked: "Enter time in HH:MM format (00:30 - 23:59)" });
      return;
    }
  
    setErrors({});
    
    const timeRegistration: TimeRegistrationDTO = {
      id: selectedTimeRegistration.id, // Ensure this is defined
      projectId: selectedProject.id,
      freelancerId: formData.freelancerId,
      workDate: selectedTimeRegistration.workDate,
      hoursWorked: convertToDecimal(formData.hoursWorked),
      description: formData.description,
      projectName: selectedProject.name,
    };
  
    console.log("DTO being sent: ", timeRegistration);
  
    await timeRegistrationService.updateTimeRegistration(timeRegistration.id, timeRegistration);
    setOpenEditTimeRegistrationModal(false);
    // Hacky re-render trigger:
    setProjects(projects.map(p => ({ ...p })));
    setSelectedDate(new Date(selectedDate!))
  };

  const handleClose = () => {
    setOpenEditTimeRegistrationModal(false);
    setErrors({});
   
  }

  return (
    <>
      <Modal isOpen={openEditTimeRegistrationModal} onClose={handleClose} isCentered>
        <ModalOverlay
          bg="rgba(0, 0, 0, 0.1)"
          backdropFilter="blur(2px)"
        />
        <ModalContent mx="auto" width={600} backgroundColor="#ffffff" borderRadius={8} padding={10}>
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

export default EditTimeRegistrationModal;
