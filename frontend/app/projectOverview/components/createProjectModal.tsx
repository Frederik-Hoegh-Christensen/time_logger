import React, { useState } from "react";
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
import type { ProjectCreateDTO } from "~/models/project";
import { useProjectContext } from "~/contexts/projectContext";
import { freelancerService } from "~/api/freelancerService";

const CreateProjectModal = () => {
  const {setProjects} = useProjectContext()
  const { open, onOpen, onClose } = useDisclosure();
  const [errors, setErrors] = useState<{name?: string, deadline?: string, customerName?: string, customerEmail?: string}>({});
  const [formData, setFormData] = useState({
    customerName: "",
    customerEmail: "",
    projectName: "",
    projectDeadline: "",
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const today = new Date().toISOString().split("T")[0];
    if (formData.projectDeadline < today || formData.projectDeadline === "") {
      setErrors({ deadline: "Choose a deadline that is not in the past" });
      return;
    }

    if (!formData.projectName || formData.projectName === "") {
      setErrors({ name: "Input a project name" });
      return;
    }

    if (!formData.customerEmail || formData.customerEmail === "") {
      setErrors({ customerEmail: "Input an email for the customer" });
      return;
    }
  
    if (!formData.customerName || formData.customerName === "") {
      setErrors({ customerName: "Input a name for the customer" });
      return;
    }
   

    

    const customer: CustomerCreateDTO = {name: formData.customerName, email: formData.customerEmail} 
    const customerId = await customerService.createCustomer(
      customer
    )
    const project: ProjectCreateDTO = {
      name: formData.projectName,
      deadline: formData.projectDeadline,
      freelancerId: "DEA192EE-B1D0-43DA-0A7D-08DD6C71F515",
      customerId: customerId,
      isCompleted: false
    }
    await projectService.createProject(project);
    var projects = await freelancerService.getProjectsByFreelancerId("DEA192EE-B1D0-43DA-0A7D-08DD6C71F515")
    setProjects(projects);
    setFormData({
      customerName: "",
      customerEmail: "",
      projectName: "",
      projectDeadline: "",});
    onClose();
  };
  return (
    <>
      <Button style={{marginLeft: 10}} onClick={onOpen}>Create new project</Button>

      <Modal isOpen={open} onClose={onClose} isCentered>
        <ModalOverlay
          bg="rgba(0, 0, 0, 0.1)" // Semi-transparent dark overlay
          backdropFilter="blur(2px)" // Blurs the background
        />
        <ModalContent mx='auto' my='300' width={600} backgroundColor='#ffffff' borderRadius={8} padding={10}>
          <ModalHeader mx='auto' mb={10}><strong>Create new project</strong> </ModalHeader>
          <ModalBody mx='auto'>
          <form onSubmit={handleSubmit} style={{ display: "flex" }}>
                {/* Project Form (Left Side) */}
                <div style={{ flex: 1, marginRight: "20px" }}>
                  <FormControl mb={4} isInvalid={!!errors.name} isRequired>
                    <FormLabel htmlFor="projectName">Project Name</FormLabel>
                    <Input
                      required
                      id="projectName"
                      name="projectName"
                      value={formData.projectName}
                      onChange={handleInputChange}
                      placeholder="Enter project name"
                    />
                    <FormErrorMessage textColor='red'>{errors.name}</FormErrorMessage>
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
                  <FormControl mb={4} isInvalid={!!errors.customerName} isRequired>
                    <FormLabel htmlFor="customerName">Customer Name</FormLabel>
                    <Input
                      required
                      id="customerName"
                      name="customerName"
                      value={formData.customerName}
                      onChange={handleInputChange}
                      placeholder="Enter customer name"
                    />
                    <FormErrorMessage textColor='red'>{errors.customerName}</FormErrorMessage>
                  </FormControl>

                  <FormControl mb={4} isInvalid={!!errors.customerEmail} isRequired>
                    <FormLabel htmlFor="customerEmail">Customer Email</FormLabel>
                    <Input
                      required
                      id="customerEmail"
                      name="customerEmail"
                      value={formData.customerEmail}
                      onChange={handleInputChange}
                      placeholder="Enter customer email"
                    />
                    <FormErrorMessage textColor='red'>{errors.customerEmail}</FormErrorMessage>
                  </FormControl>
                </div>
              </form>
          </ModalBody>

          <ModalFooter mx='auto'>
              <Button bgColor={"green"} mr={3} type="submit" onClick={handleSubmit}>
                Create
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

export default CreateProjectModal;
