import { Container, Typography, ButtonGroup, Button, AlertTitle, Alert, List, ListItem, ListItemText } from "@mui/material";
import agent from "../../app/api/agent";
import { useState } from "react";

export default function AboutPage() {
    const [validationErrors, setValidatiionErrors] = useState<string[]>([]);

    function getValidationError() {
        agent.TestErrors.getValidationError()
            .then(() => console.log('should not be seen'))
            .catch(error => setValidatiionErrors(error));
    }

    return (
        <Container>
            <Typography gutterBottom variant='h2'>Errors for testing purposes</Typography>
            <ButtonGroup fullWidth>
                <Button variant='contained' onClick={() => agent.TestErrors.get400Error().catch(error => console.log(error))}>Test 400 Error</Button>
                <Button variant='contained' onClick={() => agent.TestErrors.get401Error().catch(error => console.log(error))}>Test 401 Error</Button>
                <Button variant='contained' onClick={() => agent.TestErrors.get404Error().catch(error => console.log(error))}>Test 404 Error</Button>
                <Button variant='contained' onClick={() => agent.TestErrors.get500Error().catch(error => console.log(error))}>Test 500 Error</Button>
                <Button variant='contained' onClick={getValidationError}>Test Validation Error</Button>
            </ButtonGroup>
            {validationErrors.length > 0 &&
                <Alert severity='error'>
                    <AlertTitle>Validation Errors</AlertTitle>
                    <List>
                        {validationErrors.map(error => (
                            <ListItem key={error}>
                                <ListItemText>{error}</ListItemText>
                            </ListItem>
                        ))}
                    </List>
                </Alert>
            }
        </Container>
    )
}