import { AppBar, Toolbar, Box, Tooltip, Switch, IconButton, Badge, Typography } from "@mui/material";
import InfoIcon from '@mui/icons-material/Info';
import CallIcon from '@mui/icons-material/Call';
import QuizIcon from '@mui/icons-material/Quiz';
import LinkedInIcon from '@mui/icons-material/LinkedIn';
import FacebookIcon from '@mui/icons-material/Facebook';
import InstagramIcon from '@mui/icons-material/Instagram';
import XIcon from '@mui/icons-material/X';
import EmailIcon from '@mui/icons-material/Email';
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../store/configureStore";

const navStyles = {
    color: 'inherit',
    textDecoration: 'none',
    cursor: 'pointer',
    '&:hover': {
        color: 'grey.500'
    },
    '&.active': {
        color: 'text.secondary'
    },
    minWidth: 40,
}

const secondRowIconStyles = {
    cursor: 'pointer',
    padding: 1.5,
    borderRadius: 2,
    border: '1px solid #ddd',
    backgroundColor: '',
    transition: 'all 0.3s ease',
    '&:hover': {
        backgroundColor: 'primary',
        color: 'grey.500',
        boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.15)',
        transform: 'scale(1.1)',
    },
}

export default function Footer() {
    const navigate = useNavigate();
    const { user } = useAppSelector(state => state.account);
    return (
        <AppBar position="static" component="footer" sx={{ top: 'auto', bottom: 0 }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: 2, bottom: 0 }}>

                <Box display='flex' alignItems='center' >
                    <Tooltip title="About" >
                        <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: 0 }} onClick={() => navigate('/about')}  >
                            <InfoIcon fontSize="medium" sx={navStyles} ></InfoIcon>
                            <Typography variant="body1" sx={{ ml: 0, mr: 1 }}>About</Typography>
                        </Box>
                    </Tooltip>
                    <Tooltip title="Contact" >
                        <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: 0 }} onClick={() => navigate('/contact')}  >
                            <CallIcon fontSize="medium" sx={navStyles} ></CallIcon>
                            <Typography variant="body1" sx={{ ml:0, mr: 1 }}>Contact</Typography>
                        </Box>
                    </Tooltip>
                    <Tooltip title="Frequently asked questions" >
                        <Box display="flex" alignItems="center" sx={{ ...secondRowIconStyles, marginRight: 2, marginLeft: 0 }} onClick={() => navigate('/about')}  >
                            <QuizIcon fontSize="medium" sx={navStyles} ></QuizIcon>
                            <Typography variant="body1" sx={{ ml: 0, mr: 1 }}>FAQ</Typography>
                        </Box>
                    </Tooltip>
                </Box>

                {user &&
                    <Box display='flex' alignItems='center'>
                        <Typography variant="h6" sx={navStyles} >{user.email}</Typography>
                    </Box>
                }

                <Box display='flex' alignItems='center'>
                    <EmailIcon fontSize="medium" sx={navStyles} ></EmailIcon>
                    <LinkedInIcon fontSize="medium" sx={navStyles} ></LinkedInIcon>
                    <FacebookIcon fontSize="medium" sx={navStyles} ></FacebookIcon>
                    <InstagramIcon fontSize="medium" sx={navStyles} ></InstagramIcon>
                    <XIcon fontSize="medium" sx={navStyles} ></XIcon>
                </Box>

            </Toolbar>
        </AppBar>
    )
}