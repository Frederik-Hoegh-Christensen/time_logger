import React, { useEffect, useState, type FC } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
} from "@chakra-ui/modal";
import { Button } from "@chakra-ui/react";
import { Input } from "@chakra-ui/react";
import { FormControl, FormErrorMessage, FormLabel } from "@chakra-ui/form-control";
import { timeRegistrationService } from "~/api/timeRegistrationService";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import { useProjectContext } from "~/contexts/projectContext";
import { convertToDecimal, convertToHHMM, validateTimeFormat } from "~/utils/timeHelper";
import type { ProjectDTO } from "~/models/project";

interface IProps {
    open: boolean
    timeRegistration: TimeRegistrationDTO;
    onClose: () => void;
    onConfirm: () => void;
}
const EditTimeRegistrationModal: FC<IProps> = ({open, timeRegistration, onClose, onConfirm}) => {
  const {projects, setProjects} = useProjectContext();
  const [selectedProject, setSelectedProject] = useState<ProjectDTO | null>(null);
  const [errors, setErrors] = useState<{ hoursWorked?: string, projectId?: string }>({});
  const [formData, setFormData] = useState(() => {
    return {
      id: "",
      freelancerId: "DEA192EE-B1D0-43DA-0A7D-08DD6C71F515",
      hoursWorked: "",
      description: "",
      projectId: ""
    };
  
    
  });

  useEffect(() => {
    const timeRegistrationCurrentProject = projects.find(p => p.id == timeRegistration.projectId);
    if (timeRegistrationCurrentProject){
        setSelectedProject(timeRegistrationCurrentProject)
    }
    
  },[])

  useEffect(() => {
    setFormData({
      id: timeRegistration.id,
      freelancerId: "DEA192EE-B1D0-43DA-0A7D-08DD6C71F515",
      hoursWorked: convertToHHMM(timeRegistration.hoursWorked),
      description: timeRegistration.description ?? "",
      projectId: selectedProject?.id ?? "", // Add projectId to formData
    });
  }, [timeRegistration, selectedProject]);

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
  
    if (!validateTimeFormat(formData.hoursWorked)) {
      setErrors({ hoursWorked: "Enter time in HH:MM format (00:30 - 23:59)" });
      return;
    }
  
    setErrors({});
    
    const updatedTimeRegistration: TimeRegistrationDTO = {
      id: timeRegistration.id, // Ensure this is defined
      projectId: selectedProject.id,
      freelancerId: formData.freelancerId,
      workDate: timeRegistration.workDate,
      hoursWorked: convertToDecimal(formData.hoursWorked),
      description: formData.description,
      projectName: selectedProject.name,
    };
  
    console.log("DTO being sent: ", timeRegistration);
  
    await timeRegistrationService.updateTimeRegistration(timeRegistration.id, updatedTimeRegistration);
    // Hacky re-render trigger:
    setProjects(projects.map(p => ({ ...p })));
    onConfirm()
  };

  const handleClose = () => {
    setErrors({});
   
  }

  return (
    <>
      <Modal isOpen={open} onClose={handleClose} isCentered>
        <ModalOverlay
          bg="rgba(0, 0, 0, 0.1)"
          backdropFilter="blur(2px)"
        />
        <ModalContent mx="auto" width={600} backgroundColor="#ffffff" borderRadius={8} padding={10}>
          <ModalHeader mx="auto" mb={10}>
            <strong>Edit Time Registration</strong>
          </ModalHeader>
          <ModalBody mx="auto">
            <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column" }}>
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
            <Button bgColor={"red"} onClick={onClose}>
              Close
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
};

export default EditTimeRegistrationModal;
