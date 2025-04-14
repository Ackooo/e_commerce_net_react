import { useState } from "react";
import { useNavigate } from 'react-router-dom';
import { Box, Avatar, Menu, MenuItem, ListItemIcon, Divider, IconButton, Tooltip, Typography, Fade, } from '@mui/material';

export interface MenuOption {
    label: string | React.ReactNode;
    icon?: React.ReactNode;
    route?: string;
    onClick?: () => void;
    dividerBefore?: boolean;
    clickable?: boolean;
}

interface DropdownMenuProps {
    avatarLabel?: string;
    tooltip?: string;
    options: MenuOption[];
    triggerLabel?: string;
    customTrigger?: React.ReactNode;
    customAvatar?: string | React.ReactNode;
}

const DropdownMenu: React.FC<DropdownMenuProps> = ({ avatarLabel = 'M', tooltip = 'Menu', options, triggerLabel, customTrigger, customAvatar, }) => {
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const navigate = useNavigate();

    return (
        <Box sx={{ display: 'flex', alignItems: 'center', textAlign: 'center' }}>
            {triggerLabel && <Typography sx={{ minWidth: 100 }}>{triggerLabel}</Typography>}
            <Tooltip title={tooltip}>
                <IconButton onClick={handleClick} size="small" sx={{ ml: 2 }} aria-controls={open ? 'custom-menu' : undefined} aria-haspopup="true" aria-expanded={open ? 'true' : undefined}                >
                    {customTrigger || (
                        typeof customAvatar === 'string' ? (
                            <Avatar src={customAvatar} alt="avatar" sx={{ width: 42, height: 42 }} />
                        ) : customAvatar ? (
                            customAvatar
                        ) : (
                            <Avatar sx={{ width: 32, height: 32 }}>{avatarLabel}</Avatar>
                        )
                    )}

                </IconButton>
            </Tooltip>
            <Menu
                anchorEl={anchorEl} id="custom-menu" open={open} onClose={handleClose} onClick={handleClose} TransitionComponent={Fade}
                slotProps={{
                    paper: {
                        elevation: 0,
                        sx: {
                            overflow: 'visible', filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))', mt: 1.5,
                            '& .MuiAvatar-root': { width: 42, height: 42, ml: -0.5, mr: 1, },
                            '&::before': { content: '""', display: 'block', position: 'absolute', top: 0, right: 14, width: 10, height: 10, bgcolor: 'background.paper', transform: 'translateY(-50%) rotate(45deg)', zIndex: 0, },
                        },
                    },
                }}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }} anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >

                {options.map((option, index) => (
                    <Box key={index}>
                        {option.dividerBefore && <Divider />}
                        <MenuItem onClick={(event: React.MouseEvent<HTMLLIElement>) => { if (option.clickable) { event!.stopPropagation(); return; } if (option.route) navigate(option.route); option.onClick?.(); handleClose(); }}
                            sx={{
                                ...(option.clickable && {//pointerEvents: 'none', color: 'text.disabled',
                                    opacity: 0.5, cursor: 'default', userSelect: 'none', backgroundColor: 'transparent',
                                    '&:hover': { backgroundColor: 'transparent', },
                                    '&:active': { backgroundColor: 'transparent', }
                                }),
                            }}                            >
                            {option.icon && <ListItemIcon>{option.icon}</ListItemIcon>}
                            {option.label}
                        </MenuItem>
                    </Box >
                ))}

            </Menu>
        </Box>
    );
};

export default DropdownMenu;
