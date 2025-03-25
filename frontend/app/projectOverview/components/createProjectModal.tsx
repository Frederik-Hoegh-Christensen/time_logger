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

const CreateProjectModal = () => {
  const { open, onOpen, onClose } = useDisclosure();
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
    const customer: CustomerCreateDTO = {name: formData.customerName, email: formData.customerEmail} 
    const customerId = await customerService.createCustomer(
      customer
    )
    const project: ProjectCreateDTO = {
      name: formData.projectName,
      deadline: formData.projectDeadline,
      freelancerId: "5088F034-274E-412B-2394-08DD648C3E34",
      customerId: customerId,
      isCompleted: false
    }
    await projectService.createProject(project);
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

                  <FormControl mb={4}>
                    <FormLabel htmlFor="projectDeadline">Project Deadline</FormLabel>
                    <Input
                      required
                      id="projectDeadline"
                      name="projectDeadline"
                      type="date"
                      value={formData.projectDeadline}
                      onChange={handleInputChange}
                    />
                  </FormControl>
                </div>

                {/* Customer Form (Right Side) */}
                <div style={{ flex: 1 }}>
                  <FormControl mb={4}>
                    <FormLabel htmlFor="customerName">Customer Name</FormLabel>
                    <Input
                      required
                      id="customerName"
                      name="customerName"
                      value={formData.customerName}
                      onChange={handleInputChange}
                      placeholder="Enter customer name"
                    />
                  </FormControl>

                  <FormControl mb={4}>
                    <FormLabel htmlFor="customerEmail">Customer Email</FormLabel>
                    <Input
                      required
                      id="customerEmail"
                      name="customerEmail"
                      value={formData.customerEmail}
                      onChange={handleInputChange}
                      placeholder="Enter customer email"
                    />
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
